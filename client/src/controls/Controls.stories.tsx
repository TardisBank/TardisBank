import * as React from 'react';
import { storiesOf } from '@storybook/react';
import { Form } from './Form';

const noop = () => {};
storiesOf('Controls', module)
  .add('Form', () => (
      <Form onSubmit={noop} submit={<div>Hello</div>}>
          <div>Some form items</div>
      </Form> 
  ))