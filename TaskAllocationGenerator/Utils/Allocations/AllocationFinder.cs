using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAllocationUtils.Files;

namespace TaskAllocationGenerator.Utils.Allocations
{
    public class AllocationFinder
    {
        public ConfigurationFile Configuration { get; set; }

        public AllocationFinder()
        {

        }

        public AllocationFinder(ConfigurationFile configuration)
        {
            Configuration = configuration;
        }

        public void Run()
        {

        }
    }
}
