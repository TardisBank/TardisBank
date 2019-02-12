import * as React from 'react';
import { storiesOf } from '@storybook/react';
import { Form } from './Form';

const noop = () => {};
storiesOf('Controls', module)
  .add('Form', () => (
      <Form onSubmit={noop} submit={<div>Submit section</div>}>
          <div>This is inside the from. It shows the shared styles, which is mainly padding</div>
      </Form> 
  ))