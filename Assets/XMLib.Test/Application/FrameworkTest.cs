/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/xiangmu110/XMLib/wiki
 * 创建时间: 12/12/2018 4:18:47 PM
 */

namespace XMLib.Test.ApplicationTest
{
    public class FrameworkTest : Framework
    {
        protected override IBootstrap[] GetBootstraps()
        {
            return ArrayUtil.Merge(base.GetBootstraps(),
                new IBootstrap[] {
                    new BootstrapTest1(),
                    new BootstrapTest2()
                }
            );
        }

        protected override void OnBootstraped()
        {
            base.OnBootstraped();
        }
    }
}