using System;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Contracts;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace GovTokenNethereum
{
    class Program
    {
        public const string accountRod = "0x0F5fDfF9e9054DDda9Cc84C7b8507998E11124b6";
        public const string accountRafa = "0x412A814777Ed7509C3d1C49b1aea8bb2D49eC272";
        public const string accountCustody = "0x17c7B9B3ef7D7860Dc6e2126D6380680A9CDcF55";

        static async Task Main(string[] args)
        {
            AccessGovTokenContract accGT = new AccessGovTokenContract();

            var owner = await accGT.GetOwner();
            Console.WriteLine("Owner of contract: " + owner);

            var balanceOf = await accGT.GetBalanceOf(accountRafa);
            Console.WriteLine("Balance before transfer: " + balanceOf);

            await accGT.Transfer(accountRafa, 1000000);
            balanceOf = await accGT.GetBalanceOf(accountRafa);
            Console.WriteLine("Balance after transfer: " + balanceOf);

            await accGT.ChangeOwner(accountRafa);
            owner = await accGT.GetOwner();
            Console.WriteLine("New owner of contract: " + owner);
        }
    }

    [Function("changeOwner", "bool")]
    public class ChangeOwnerFunction : FunctionMessage
    {
        [Parameter("address", "_newOwner", 1)]
        public string NewOwner { get; set; }
    }

    [Function("transfer", "bool")]
    public class TransferFunction : FunctionMessage
    {
        [Parameter("address", "recipient", 1)]
        public string Recipient { get; set; }

        [Parameter("uint256", "amount", 2)]
        public BigInteger Amount { get; set; }
    }

    [Event("Transfer")]
    public class TransferEventDTO : IEventDTO
    {
        [Parameter("address", "from", 1, true)]
        public string From { get; set; }

        [Parameter("address", "to", 2, true)]
        public string To { get; set; }

        [Parameter("uint256", "value", 3, false)]
        public BigInteger Value { get; set; }
    }

    class AccessGovTokenContract
    {
        private Contract contract;
        private Web3 web3;

        public AccessGovTokenContract()
        {
            var url = "https://ropsten.infura.io/v3/8c6d2c33a9744049837aaaf0b2d8276f";
            var privateKey = "0x16ac9796f4c062cd1ecbf042c6dce04e644f10e5995694e12e7528af8c37eeb3";
            var account = new Account(privateKey);
            web3 = new Web3(account, url);

            var abi = @"[
	{
		""inputs"": [],
		""stateMutability"": ""nonpayable"",
		""type"": ""constructor""
	},
	{
		""anonymous"": false,
		""inputs"": [
			{
				""indexed"": true,
				""internalType"": ""address"",
				""name"": ""owner"",
				""type"": ""address""
			},
			{
				""indexed"": true,
				""internalType"": ""address"",
				""name"": ""spender"",
				""type"": ""address""
			},
			{
				""indexed"": false,
				""internalType"": ""uint256"",
				""name"": ""value"",
				""type"": ""uint256""
			}
		],
		""name"": ""Approval"",
		""type"": ""event""
	},
	{
		""anonymous"": false,
		""inputs"": [
			{
				""indexed"": true,
				""internalType"": ""address"",
				""name"": ""from"",
				""type"": ""address""
			},
			{
				""indexed"": true,
				""internalType"": ""address"",
				""name"": ""to"",
				""type"": ""address""
			},
			{
				""indexed"": false,
				""internalType"": ""uint256"",
				""name"": ""value"",
				""type"": ""uint256""
			}
		],
		""name"": ""Transfer"",
		""type"": ""event""
	},
	{
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": ""owner"",
				""type"": ""address""
			},
			{
				""internalType"": ""address"",
				""name"": ""spender"",
				""type"": ""address""
			}
		],
		""name"": ""allowance"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": ""spender"",
				""type"": ""address""
			},
			{
				""internalType"": ""uint256"",
				""name"": ""amount"",
				""type"": ""uint256""
			}
		],
		""name"": ""approve"",
		""outputs"": [
			{
				""internalType"": ""bool"",
				""name"": """",
				""type"": ""bool""
			}
		],
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": ""account"",
				""type"": ""address""
			}
		],
		""name"": ""balanceOf"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""inputs"": [
			{
				""internalType"": ""uint256"",
				""name"": ""_amount"",
				""type"": ""uint256""
			}
		],
		""name"": ""burn"",
		""outputs"": [
			{
				""internalType"": ""bool"",
				""name"": """",
				""type"": ""bool""
			}
		],
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""inputs"": [
			{
				""internalType"": ""address payable"",
				""name"": ""_newOwner"",
				""type"": ""address""
			}
		],
		""name"": ""changeOwner"",
		""outputs"": [],
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""inputs"": [],
		""name"": ""decimals"",
		""outputs"": [
			{
				""internalType"": ""uint8"",
				""name"": """",
				""type"": ""uint8""
			}
		],
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""inputs"": [
			{
				""internalType"": ""uint256"",
				""name"": ""_amount"",
				""type"": ""uint256""
			}
		],
		""name"": ""mint"",
		""outputs"": [
			{
				""internalType"": ""bool"",
				""name"": """",
				""type"": ""bool""
			}
		],
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""inputs"": [],
		""name"": ""name"",
		""outputs"": [
			{
				""internalType"": ""string"",
				""name"": """",
				""type"": ""string""
			}
		],
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""inputs"": [],
		""name"": ""owner"",
		""outputs"": [
			{
				""internalType"": ""address payable"",
				""name"": """",
				""type"": ""address""
			}
		],
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""inputs"": [],
		""name"": ""symbol"",
		""outputs"": [
			{
				""internalType"": ""string"",
				""name"": """",
				""type"": ""string""
			}
		],
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""inputs"": [],
		""name"": ""totalSupply"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": ""recipient"",
				""type"": ""address""
			},
			{
				""internalType"": ""uint256"",
				""name"": ""amount"",
				""type"": ""uint256""
			}
		],
		""name"": ""transfer"",
		""outputs"": [
			{
				""internalType"": ""bool"",
				""name"": """",
				""type"": ""bool""
			}
		],
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": ""sender"",
				""type"": ""address""
			},
			{
				""internalType"": ""address"",
				""name"": ""recipient"",
				""type"": ""address""
			},
			{
				""internalType"": ""uint256"",
				""name"": ""amount"",
				""type"": ""uint256""
			}
		],
		""name"": ""transferFrom"",
		""outputs"": [
			{
				""internalType"": ""bool"",
				""name"": """",
				""type"": ""bool""
			}
		],
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	}
]";
            var contractAddress = "0x9b6262Bf836e1EE3CF0fB5B669d04861Ef4D686f"; // endereço do contrato já feito deploy na rede (W3G)

            contract = web3.Eth.GetContract(abi, contractAddress);
        }

        public async Task<string> GetOwner()
        {
            var getOwnerFunction = contract.GetFunction("owner");
            var owner = await getOwnerFunction.CallAsync<string>();

            return owner;
        }

        public async Task<BigInteger> GetBalanceOf(string _account)
        {
            var getBalanceOfFunction = contract.GetFunction("balanceOf");
            var balance = await getBalanceOfFunction.CallAsync<BigInteger>(_account);

            return balance;
        }

        public async Task Transfer(string _recipient, BigInteger _amount)
        {
            var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();
            var transfer = new TransferFunction()
            {
                Recipient = _recipient,
                Amount = _amount
            };
            var transactionReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync(contract.Address, transfer);
        }

        public async Task ChangeOwner(string _newOwner)
        {
            var changeOwnerHandler = web3.Eth.GetContractTransactionHandler<ChangeOwnerFunction>();
            var changeOwner = new ChangeOwnerFunction()
            {
                NewOwner = _newOwner
            };
            var transactionReceipt = await changeOwnerHandler.SendRequestAndWaitForReceiptAsync(contract.Address, changeOwner);
        }
    }

}
