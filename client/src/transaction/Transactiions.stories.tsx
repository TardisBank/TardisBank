import * as React from "react";
import { storiesOf } from "@storybook/react";
import { AddTransaction } from "./AddTransaction";
import { ListTransactions } from "./ListTransactions";
import { Transactions, Direction } from "../model";

const log = (s: any, direction: Direction) =>
  console.log(`Amount: ${s} direction: ${direction}`);

const someTransactions: Transactions = Array.from({ length: 3 }).map(
  (x: any, i: any) => {
    return {
      id: i,
      accountId: "1",
      amount: (i + 1) * 200,
      date: new Date(2018, 0, i + 1, 0, 0, 0, 0),
      balance: 100,
      operations: { self: "1" }
    };
  }
);

storiesOf("Transactions", module)
  .add("add transaction", () => <AddTransaction addTransaction={log} />)
  .add("list transactions", () => (
    <ListTransactions transactions={someTransactions} />
  ));
