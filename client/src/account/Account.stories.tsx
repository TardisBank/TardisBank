import * as React from 'react';
import { storiesOf } from '@storybook/react';
import { AddAccount } from './AddAccount';

const noop = () => {}

storiesOf('Accounts', module)
  .add('add account', () => (
      <AddAccount onAddAccount={noop} />
  ))