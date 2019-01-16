import * as React from 'react';
import { storiesOf } from '@storybook/react';
import { AddTransaction } from './AddTransactions';
import { ListTransactions } from './ListTransactions';
import { Transactions } from 'src/model';

const log = (s: any) => console.log(s);

const someTransactions: Transactions = Array.from({ length: 3 }).map((x: any, i: any) => {
  return {
    id: i,
    accountId: '1',
    amount: (i + 1) * 200,
    date: new Date(2018, 0, (i +1), 0 , 0, 0, 0)
  }
})

storiesOf('Transactions', module)
  .add('add transaction', () => (
    <AddTransaction addTransaction={log} />
  ))
  .add('list transactions', () => <ListTransactions transactions={someTransactions} />);
