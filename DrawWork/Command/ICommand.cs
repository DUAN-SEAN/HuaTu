namespace DrawWork.Command
{
    public  interface ICommand
    {
        void Execute();//提交操作
        void UnExecute();//取消操作
    }
}