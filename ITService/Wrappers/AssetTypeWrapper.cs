using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITService.Model;

namespace ITService.UI.Wrappers
{
    class AssetTypeWrapper : ModelWrapper<AssetType>
    {
        public AssetTypeWrapper(AssetType model) : base(model)
        {
        }
        public int Id => Model.Id;
        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}
