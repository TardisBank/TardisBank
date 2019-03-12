import * as React from "react";
import { AddTransaction } from "./AddTransaction";
import {
  ListTransactionContainer,
  ListTransactionProps
} from "./ListTransactionsContainer";

export type TransactionHomeProps = ListTransactionProps;

const noop = () => {};

export const Transactions: React.FunctionComponent<
  TransactionHomeProps
> = props => (
  <>
    <h2>Transactions for {props.account.name}</h2>
    <AddTransaction addTransaction={noop} />
    <ListTransactionContainer {...props} />
  </>
);
