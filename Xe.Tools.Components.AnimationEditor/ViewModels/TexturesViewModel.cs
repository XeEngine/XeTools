using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xe.Game;

namespace Xe.Tools.Components.AnimationEditor.ViewModels
{
    public class TexturesViewModel : INotifyPropertyChanged
    {
        private List<Texture> _textures { get; set; }
        private string BasePath { get; set; }
        private ObservableCollection<TextureViewModel> _textureVm;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TextureViewModel> Textures
        {
            get => _textureVm;
            private set
            {
                _textureVm = value;
                OnPropertyChanged();
            }
        }

        public int Count => Textures.Count;

        public TexturesViewModel(List<Texture> textures, string basePath)
        {
            _textures = textures;
            BasePath = basePath;
            Textures = new ObservableCollection<TextureViewModel>(_textures.Select(x =>
            {
                return new TextureViewModel(x, BasePath);
            }));
        }

        public int AddTexture(string fileName)
        {
            return AddTexture(GetDefaultTexture(fileName));
        }
        public int AddTexture(Texture texture)
        {
            Textures.Add(new TextureViewModel(texture, BasePath));
            return Textures.Count - 1;
        }
        public void ReplaceTexture(int index, string fileName)
        {
            ReplaceTexture(index, GetDefaultTexture(fileName));
        }
        public void ReplaceTexture(int index, Texture texture)
        {
            if (index >= 0 && index < Count)
            {
                Textures[index] = new TextureViewModel(texture, BasePath);
            }
            else
            {
                Log.Error($"Unable to insert {texture.Name} because index {index} is invalid");
            }
        }
        public void RemoveTexture(int index, bool physicalDelete)
        {
            if (index >= 0 && index < Count)
            {
                if (physicalDelete)
                {
                    var item = Textures[index];
                    var fileName = item.Texture.Name;
                    var filePath = Path.Combine(BasePath, fileName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        Log.Message($"File {fileName} was deleted");
                    }
                    else
                    {
                        Log.Warning($"File {fileName} was not deleted because it does not exists.");
                    }
                }
                Textures.RemoveAt(index);
            }
            else
            {
                Log.Error($"Unable to remove the item because index {index} is invalid");
            }
        }

        public void SaveChanges()
        {
            _textures.Clear();
            _textures.AddRange(Textures.Select(x => x.Texture));
        }

        /// <summary>
        /// Create default entry for a texture from the specified file name
        /// </summary>
        /// <param name="fileName">Name of the texture file</param>
        /// <returns></returns>
        private Texture GetDefaultTexture(string fileName)
        {
            return new Texture()
            {
                Id = Guid.NewGuid(),
                Name = fileName,
                MaintainPaletteOrder = true
            };
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
