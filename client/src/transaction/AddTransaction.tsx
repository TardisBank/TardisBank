import * as React from 'react';
import { TextField, Button, Typography } from '@material-ui/core';
import { Form } from '../controls/index';

export type AddTransactionDispatchProps = {
    addTransaction: (amount: number) => void;
}

export type AddTransactionState = {
    amount?: string;
    amountError: boolean;
}

export type AddTransactionProps = AddTransactionDispatchProps;

export class AddTransaction extends React.Component<AddTransactionProps> {
    state = {
        amount: undefined,
        amountError: false,
    } as AddTransactionState;

    handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            ...this.state,
            [event.target.name]: event.target.value
        })
    }

    private handleSubmit = (event: React.FormEvent) => {

        const { amount } = this.state;
        if(!amount) {
            this.setState({
                ...this.state,
                amountError: true
            });
            return;
        }
        const value = parseFloat(amount);
        
        if(Number.isNaN(value)) {
            this.setState({
                ...this.state,
                amountError: true
            });
            return;
        }

        this.props.addTransaction(value);
        this.setState({
            ...this.state,
            amountError: false 
        });
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
                    Add transaction
                </Typography>
                <Form onSubmit={this.handleSubmit}
                    submit={submitButton}
                >
                    <TextField
                        name="amount"
                        id='amount'
                        label="Amount"
                        fullWidth={true}
                        required={true}

                        error={this.state.amountError}
                        onChange={this.handleChange}
                    />
                </Form>
            </main>
        )
    }
}