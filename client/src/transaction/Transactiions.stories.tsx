import * as React from 'react';
import { storiesOf } from '@storybook/react';
import { AddTransaction } from './AddTransactions';

storiesOf('Transactions', module)
  .add('add transaction', () => (
    <AddTransaction />
  ))