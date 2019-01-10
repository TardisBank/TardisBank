import * as React from 'react';
import { TextField, Button, Typography } from '@material-ui/core';
import { Form } from '../controls';

export class AddTransaction extends React.Component {

    private handleSubmit = (event: React.FormEvent) => {

    }

    render() {
        const submitButton = <Button
            type="submit"
            color="primary"
            variant="raised"
            name="Save">
            Add
        </Button>;

        return (
            <main>
                <Typography>
                    Add account for your child
                </Typography>
                <Form onSubmit={this.handleSubmit}
                    submit={submitButton}
                >
                    <TextField
                        name="amount"
                        id='amount'
                        label="Amount"
                        fullWidth={true}
                    />
                </Form>
            </main>
        )
    }
}