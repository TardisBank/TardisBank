import * as React from 'react';
import { Account, Transaction } from '../model'
import { ListTransactions } from './ListTransactions';
import { getMessagingClient } from 'src/messaging';
import { TransactionResponseCollection } from 'tardis-bank-dtos';
import { fromTransactionResponseToTransaction } from 'src/messaging/adapters';

type ListTransactionStateProps = {
    account: Account
}

type ListTransactionContainerState = {
    previousAccountId?: string;
    transactions?: ReadonlyArray<Transaction>
}

type ListTransactionProps = ListTransactionStateProps

export class ListTransactionContainer extends React.Component<ListTransactionProps, ListTransactionContainerState> {

    state = {
        previousAccountId: undefined,
        transactions: []
    };

    componentDidMount() {
        this.loadData(this.props.account.operations.transactions);
    }

    componentDidUpdate = (prevProps: ListTransactionProps, prevState: ListTransactionContainerState) => {
        if(!this.state.transactions) {
            this.loadData(this.props.account.operations.transactions);
        }
    }

    static getDerivedStateFromProps = (props : ListTransactionProps, state: ListTransactionContainerState) : ListTransactionContainerState  | null => {
        if(props.account.id !== state.previousAccountId) {
            return {
                transactions: undefined,
                previousAccountId: props.account.id
            }
        }

        return null;
    }

    loadData = (requestPath: string) => 
        getMessagingClient().get<TransactionResponseCollection>(`api/${requestPath}`)
            .then((response:TransactionResponseCollection) => {
                const transactions = response.Transactions.map((x) => fromTransactionResponseToTransaction(x))
                this.setState(state => {
                    return {
                        ...state,
                        transactions
                    }
                })
            })


    render() {

        const { transactions } = this.state;
        return (
           transactions ? <ListTransactions transactions={transactions} {...this.props} /> : <div>No transactions</div>
        )
    }
}