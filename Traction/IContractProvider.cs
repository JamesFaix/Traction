using System;
using System.Collections.Generic;

namespace Traction {

    internal interface IContractProvider {

        IEnumerable<Contract> Contracts { get; }

        Contract this[Type attributeType] { get; }
    }
}
