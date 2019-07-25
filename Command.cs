using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Command
{
    internal delegate void CommandHandler();

    internal class Command
    {
        #region 字段

        private CommandHandler _handler = null;

        #endregion 字段

        #region 构造器

        public Command()
        {
        }

        #endregion 构造器

        #region 公开函数

        public CommandHandler Handler
        {
            get { return _handler; }
            set { _handler = value; }
        }

        #endregion 公开函数
    }

}
