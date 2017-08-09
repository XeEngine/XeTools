using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Components.AnimationEditor.ViewModels
{
    public class EnumViewModel<T> where T : struct, IConvertible
    {
        public IEnumerable<Tuple<T, string>> Values { get; private set; }

        public Tuple<T, string> SelectedItem { get; set; }

        public T SelectedValue { get; set; }

        public EnumViewModel()
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new InvalidOperationException($"{type} is not an enum.");

            Values = Enum.GetValues(type)
                .Cast<T>()
                .Select(e => new Tuple<T, string>(e, e.ToString()));
        }
    }
}
