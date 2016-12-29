using System.Collections.Generic;
using System.Linq;

namespace Traction.Contracts {

    public class MasterContractProvider : IContractProvider {

        static MasterContractProvider() {
            Instance = new MasterContractProvider();
        }

        public static MasterContractProvider Instance { get; }

        public IEnumerable<Contract> Contracts =>
            DefaultValueContractProvider.Instance.Contracts.Concat(
            BasicComparisonContractProvider.Instance.Contracts);
    }
}
