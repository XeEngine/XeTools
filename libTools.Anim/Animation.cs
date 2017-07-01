using Newtonsoft.Json;
using Xe;

namespace libTools.Anim
{
    public class Animation : IDeepCloneable
    {
        [JsonIgnore]
        private FrameSequence _Sequence = new FrameSequence();

        public string Name = "<noname>";
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Link = null;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FrameSequence Sequence
        {
            get
            {
                if (Link == null) return _Sequence;
                else return null;
            }
            set
            {
                _Sequence = value;
            }
        }

        public Animation MergeWidth(Animation sequence)
        {
            Sequence.MergeWidth(sequence.Sequence);
            return this;
        }

        public override string ToString()
        {
            if (Link == null) return Name;
            return string.Format("*{0}", Name);
        }

        public object DeepClone()
        {
            var item = new Animation();
            item.Name = Name != null ? Name.Clone() as string : null;
            item.Link = Link;
            item.Sequence = Sequence.DeepClone() as FrameSequence;
            return item;
        }
    }
}
