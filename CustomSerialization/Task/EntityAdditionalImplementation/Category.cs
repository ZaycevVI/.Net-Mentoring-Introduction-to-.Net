using System;
using System.Data.Entity.Infrastructure;
using System.Runtime.Serialization;

namespace Task.DB
{
    [Serializable]
    
    public partial class Category
    {
        [OnSerializing]
        public void OnSerializing(StreamingContext context)
        {
            var objectContext = (context.Context as IObjectContextAdapter).ObjectContext;
            objectContext.LoadProperty(this, f => f.Products);
            Picture = null; // это поле занимает слишком много места в output окне, задизейблил его
        }
    }
}
