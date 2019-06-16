import * as React from "react";
import {
  Paper,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody
} from "@material-ui/core";
import { Transactions, Transaction } from "../model";

export type ListTransactionsProps = {
  transactions: Transactions;
};

const formatToCurrency = (amount: number) => {
  const formatter = new Intl.NumberFormat("en-GB", {
    style: "currency",
    currency: "GBP",
    maximumFractionDigits: 2
  });

  return formatter.format(amount);
};

export const ListTransactions: React.FunctionComponent<
  ListTransactionsProps
> = (props: ListTransactionsProps) => (
  <Paper style={{ marginTop: "20px" }}>
    <Table>
      <TableHead>
        <TableRow>
          <TableCell>Date</TableCell>
          <TableCell align="right">Amount</TableCell>
          <TableCell align="right">Balance</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        {props.transactions.map((row: Transaction) => (
          <TableRow id={row.id}>
            <TableCell>
              {row.date.toLocaleDateString()} : {row.date.toLocaleTimeString()}
            </TableCell>
            <TableCell>{formatToCurrency(row.amount)}</TableCell>
            <TableCell>{formatToCurrency(row.balance)}</TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  </Paper>
);
