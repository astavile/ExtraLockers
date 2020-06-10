using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetProtocol.Lockers
{
    public enum ReadWriteLockMode
    {
        /// <summary>
        /// Режим только чтения
        /// </summary>
        Read,

        /// <summary>
        /// Режим только записи
        /// </summary>
        Write,

        /// <summary>
        /// Обновляемый режим.
        /// Предназначен для ситуаций, где поток обычно выполняет чтение из защищенного ресурса, но может потребоваться запись в него.
        /// </summary>
        UpgradeableRead
    }
}
