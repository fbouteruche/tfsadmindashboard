using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSAdminDropFolderRights.Tests
{
    [TestFixture]
    public class DropFolderRightTest
    {
        
        [TestCase]
        public void TestProjectAndUserEnumeration()
        {
            DropFolderRightsManager manager = new DropFolderRightsManager();
            manager.SetDropRights();
        }
    }
}
