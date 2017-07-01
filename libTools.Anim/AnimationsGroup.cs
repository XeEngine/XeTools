using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace libTools.Anim
{
    public partial class AnimationsGroup : IO<AnimationsGroup>
    {
        public List<string> SpriteSheet = new List<string>();
        public Dictionary<string, Frame> Frames = new Dictionary<string, Frame>();
        public BindingList<Animation> Animations = new BindingList<Animation>();

        public AnimationsGroup()
        {
        }
        public AnimationsGroup(string filename) : base(filename)
        {
            foreach (var item in Frames)
                item.Value.Name = item.Key;
        }
        public AnimationsGroup(FileStream stream) : base(stream)
        {
            foreach (var item in Frames)
                item.Value.Name = item.Key;
        }

        protected override void Import(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void MyLoad(AnimationsGroup item)
        {
            if (item == null) return;
            SpriteSheet = item.SpriteSheet;
            Frames = item.Frames;
            Animations = item.Animations;
        }

        public Animation GetAnimation(string name)
        {
            var result = Animations.Where(x => string.Compare(x.Name, name) == 0);
            return result.Count() > 0 ? result.First() : null;
        }

        public void MergeWith(AnimationsGroup animGroup)
        {
            // Fusiona la lista di textures
            int indexTexture = 0;
            var dicTexture = new Dictionary<string, int>();
            foreach (var filename in SpriteSheet)
                dicTexture.Add(filename, indexTexture++);

            for (int i = 0; i < animGroup.SpriteSheet.Count; i++)
            {
                var filename = animGroup.SpriteSheet[i];

                int index;
                if (!dicTexture.TryGetValue(filename, out index))
                {
                    index = indexTexture;
                    dicTexture.Add(filename, indexTexture++);
                }

                // Scorre tutte le animazioni
                for (int j = 0; j < animGroup.Animations.Count;)
                {
                    // Corregge il cambio di texture
                    var item = animGroup.Animations[j];
                    if (item.Sequence.Texture == i) {
                        item.Sequence.Texture = index;
                        animGroup.Animations.RemoveAt(j);

                        // Fusiona le animazioni
                        bool isAnimationFound = false;
                        foreach (var animation in Animations)
                        {
                            // Se l'animazione c'è, allora la fusiona
                            if (string.Compare(animation.Name, item.Name, true) == 0)
                            {
                                animation.Sequence.Texture = item.Sequence.Texture;
                                animation.MergeWidth(item);
                                isAnimationFound = true;
                                break;
                            }
                        }
                        // Se l'animazione non è stata la trovata, la aggiunge ex-novo
                        if (!isAnimationFound) Animations.Add(item);
                    }
                    else j++;
                }
            }
            foreach (var frame in animGroup.Frames.Values)
            {
                Frame curFrame;
                if (Frames.TryGetValue(frame.Name, out curFrame))
                {
                    curFrame.Left = frame.Left;
                    curFrame.Top = frame.Top;
                    curFrame.Right = frame.Right;
                    curFrame.Bottom = frame.Bottom;
                }
                else
                {
                    var size = frame.Size;
                    frame.CenterX = size.Width / 2;
                    frame.CenterY = size.Height / 2;
                    Frames.Add(frame.Name, frame);
                }
            }
            var list = Animations.OrderBy(x => x.Name).ToList();
            SpriteSheet = dicTexture.Keys.ToList();
            Frames = Frames.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            Animations = new BindingList<Animation>(list);
        }
        
        /*public static AnimationsGroup FromTexturePacker(IEnumerable<Texture.TexturePacker.Item> items, string spriteSheetFilename) {
            var dicAnim = new Dictionary<string, Animation>();
            var dicFrames = new Dictionary<string, Frame>();
            var framesList = items.OrderBy(x => x.Image.Name);
            foreach (var item in framesList)
            {
                var name = item.Image.Name; // Nome del frame
                var strAnim = name.Split('#', '.'); // 
                var strName = strAnim[0]; // Nome dell'animazione appartenente

                // Ora ottiene il numero di frame dell'animazione
                int nAnim;
                if (strAnim.Length > 1)
                {
                    if (int.TryParse(strAnim[1], out nAnim))
                        nAnim -= 1;
                    else nAnim = -1;
                }
                else nAnim = -1;

                Animation animation;
                if (!dicAnim.TryGetValue(strName, out animation))
                {
                    animation = new Animation();
                    animation.Name = strName;
                    animation.Sequence.HitboxLeft = -8;
                    animation.Sequence.HitboxTop = -8;
                    animation.Sequence.HitboxRight = +8;
                    animation.Sequence.HitboxBottom = +8;
                    animation.Sequence.Loop = 0;
                    animation.Sequence.Texture = 0;
                    animation.Sequence.FramesPerSecond = 0;
                    dicAnim.Add(strName, animation);
                }

                if (nAnim < 0) nAnim = animation.Sequence.Frames.Count;

                var frame = new Frame();
                frame.Name = name;
                frame.Rectangle = item.Rectangle;
                frame.CenterX = item.Rectangle.Width / 2;
                frame.CenterY = item.Rectangle.Height / 2;
                dicFrames.Add(name, frame);

                if (nAnim < animation.Sequence.Frames.Count)
                    animation.Sequence.Frames[nAnim] = frame.Name;
                else if (nAnim > animation.Sequence.Frames.Count)
                {
                    animation.Sequence.Frames.AddRange(new string[nAnim - animation.Sequence.Frames.Count]);
                    animation.Sequence.Frames.Add(frame.Name);
                }
                else
                    animation.Sequence.Frames.Add(frame.Name);
            }

            bool createEmptyFrame = false;
            foreach (var animPair in dicAnim)
            {
                var anim = animPair.Value;
                if (anim.Sequence.Frames[0] == null)
                {
                    createEmptyFrame = true;
                    anim.Sequence.Frames[0] = "Empty";
                }
                for (int i = 1; i < anim.Sequence.Frames.Count; i++)
                {
                    if (anim.Sequence.Frames[i] == null)
                        anim.Sequence.Frames[i] = anim.Sequence.Frames[i - 1];
                }
            }
            if (createEmptyFrame)
            {
                var frame = new Frame();
                frame.Name = "Empty";
                dicFrames.Add(frame.Name, frame);
            }
            var animgroup = new AnimationsGroup();
            var list = dicAnim.Values.OrderBy(x => x.Name).ToList();
            animgroup.Frames = dicFrames;
            animgroup.Animations = new BindingList<Animation>(list);
            animgroup.SpriteSheet.Add(spriteSheetFilename);
            return animgroup;
        }*/
    }
}
