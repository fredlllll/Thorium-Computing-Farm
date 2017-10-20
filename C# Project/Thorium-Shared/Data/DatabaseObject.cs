using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thorium_Shared.Data
{
    public abstract class DatabaseObject
    {
        [DataMember, PrimaryKey, AutoIncrement]
        public long ___Id___
        {
            get;
            protected set;
        } = -1;

        public Serializer Serializer { get; set; }

        /// <summary>
        /// saves the contents of the object to the database.
        /// this method will try to save, and when encountering an interrupt, will save in the error handler and raise the exception again
        /// </summary>
        public void Save()
        {
            try
            {
                Serializer.Save(this);
            }
            catch(ThreadInterruptedException)
            {
                Serializer.Save(this);
                throw;
            }
        }
    }
}
