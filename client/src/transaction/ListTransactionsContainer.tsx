import * as React from "react";
import { Account, Transaction, Direction } from "../model";
import { ListTransactions } from "./ListTransactions";
import { getMessagingClient } from "src/messaging";
import {
  TransactionResponseCollection,
  TransactionRequest,
  TransactionResponse
} from "tardis-bank-dtos";
import { fromTransactionResponseToTransaction } from "src/messaging/adapters";
import { RequestStatus } from "src/account";
import { AddTransaction } from "./index";

type ListTransactionStateProps = {
  account: Account;
};

type ListTransactionContainerState = {
  previousAccountId?: string;
  transactions?: ReadonlyArray<Transaction>;
  addTransactionStatus: RequestStatus;
};

export type ListTransactionProps = ListTransactionStateProps;

export class ListTransactionContainer extends React.Component<
  ListTransactionProps,
  ListTransactionContainerState
> {
  state = {
    previousAccountId: undefined,
    transactions: [],
    addTransactionStatus: RequestStatus.Ready
  };

  componentDidMount() {
    this.loadData(this.props.account.operations.transactions);
  }

  componentDidUpdate = (
    prevProps: ListTransactionProps,
    prevState: ListTransactionContainerState
  ) => {
    if (!this.state.transactions) {
      this.loadData(this.props.account.operations.transactions);
    }
  };

  static getDerivedStateFromProps = (
    props: ListTransactionProps,
    state: ListTransactionContainerState
  ): ListTransactionContainerState | null => {
    if (props.account.id !== state.previousAccountId) {
      return {
        transactions: undefined,
        previousAccountId: props.account.id,
        addTransactionStatus: RequestStatus.Ready
      };
    }

    return null;
  };

  loadData = (requestPath: string) =>
    getMessagingClient()
      .get<TransactionResponseCollection>(requestPath)
      .then((response: TransactionResponseCollection) => {
        const transactions = response.Transactions.map(x =>
          fromTransactionResponseToTransaction(x)
        );
        this.setState(state => {
          return {
            ...state,
            transactions
          };
        });
      });

  handleAddTransaction = (amount: number, direction: Direction) => {
    this.setState({ addTransactionStatus: RequestStatus.Loading });
    const Amount = direction === Direction.Withdraw ? amount * -1 : amount;
    getMessagingClient()
      .post<TransactionRequest, TransactionResponse>(
        this.props.account.operations.transactions,
        {
          Amount
        }
      )
      .then(response => {
        this.setState(state => {
          const transaction = fromTransactionResponseToTransaction(response);
          const transactions = [...this.state.transactions, transaction];
          return {
            ...state,
            transactions,
            registerProcess: RequestStatus.Sent
          };
        });
      })
      .catch(() => {
        this.setState(state => {
          return {
            ...state,
            registerProcess: RequestStatus.Error
          };
        });
      });
  };

  render() {
    let { transactions } = this.state;
    const { account } = this.props;

    // TODO: Manage the sorting in state not here.
    if (transactions) {
      transactions = transactions.sort((a: Transaction, b: Transaction) => {
        if (a.date === b.date) {
          return 0;
        }
        return a.date > b.date ? -1 : 1;
      });
    }

    return transactions ? (
      <>
        <h2>Transactions for {account.name}</h2>
        <AddTransaction addTransaction={this.handleAddTransaction} />
        <ListTransactions transactions={transactions} {...this.props} />
      </>
    ) : (
      <div>No transactions</div>
    );
  }
}
