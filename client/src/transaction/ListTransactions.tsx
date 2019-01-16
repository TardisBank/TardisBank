import * as React from 'react';
import { Paper, Table, TableHead, TableRow, TableCell, TableBody } from '@material-ui/core';
import { Transactions, Transaction } from 'src/model';

export type ListTransactionsProps = {
    transactions: Transactions;
}

export const ListTransactions : React.FunctionComponent<ListTransactionsProps> = (props: ListTransactionsProps) => 
    <Paper>
        <div>Rows: {props.transactions.length}</div>
        <Table>
            <TableHead>
                <TableRow>
                    <TableCell>Date</TableCell>
                    <TableCell align="right">Amount</TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
                {props.transactions.map((row: Transaction) => (
                    <TableRow id={row.id}>
                        <TableCell>{row.date.toDateString()}</TableCell>
                        <TableCell>{row.amount}</TableCell>
                    </TableRow>
                ))}
            </TableBody>
        </Table>
    </Paper>