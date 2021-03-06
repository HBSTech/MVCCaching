﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCCaching.Kentico.Example
{
    public class KenticoExampleModuleClassRepository : IExampleModuleClassRepository
    {
        #region "These are abstracted out to Generic Models"

        // Explicit Cache Dependency, along with the No Site and No Culture as this module does not have either
        [CacheDependency("example.examplemoduleclass|byid|{0}")]
        [CacheNameNoSite]
        [CacheNameNoCulture]
        public ExampleModuleClassModel GetExampleModuleClass(int ID)
        {
            var ExampleModuleClass = ExampleModuleClassInfoProvider.GetExampleModuleClassInfo(ID);

            return new ExampleModuleClassModel()
            {
                Name = ExampleModuleClass?.ExampleModuleClassName
            };
        }

        public IEnumerable<ExampleModuleClassModel> GetExampleModuleClasses()
        {
            var ModelClasses = ExampleModuleClassInfoProvider.GetExampleModuleClasses()
                .ToList();

            return ModelClasses.Select(x =>
            {
                return new ExampleModuleClassModel()
                {
                    Name = x.ExampleModuleClassName
                };
            });
        }

        #endregion

        #region "These are returning BaseInfo/TreeNode Models"

        /// <summary>
        /// No Cache needed as the Interceptor will detect the return type is IEnumerable of BaseInfo and add the "objecttype|all" dependency
        /// </summary>
        /// <returns>All the Example Module Class Info objects</returns>
        public IEnumerable<ExampleModuleClassInfo> GetExampleModuleClasses_BaseInfo()
        {
            return ExampleModuleClassInfoProvider.GetExampleModuleClasses().ToList();
        }

        /// <summary>
        /// No Cache 'needed' however basic detection of it will simply put the "nodes|sitename|classname|all", so i'm adding one
        /// </summary>
        /// <param name="ID">The ExampleModuleClassID</param>
        /// <returns>The Example Module Class</returns>
        [CacheDependency(ExampleModuleClassInfo.OBJECT_TYPE+"|byid|{0}")]
        public ExampleModuleClassInfo GetExampleModuleClass_BaseInfo(int ID)
        {
            return ExampleModuleClassInfoProvider.GetExampleModuleClassInfo(ID);
        }

        #endregion
    }
}
