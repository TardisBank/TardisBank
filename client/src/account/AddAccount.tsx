import * as React from 'react';
import { TextField, Button, Typography, withStyles, WithStyles } from '@material-ui/core';
import {styles} from './AddAccount.styles';

type AddAccountState = {
    accountName?: string
}

export type AddAccountDispatchProps = {
    onAddAccount: (accountName: string) => void;
}

export type AddAccountProps = AddAccountDispatchProps & WithStyles<typeof styles>;
class AddAccountBase extends React.Component<AddAccountProps, AddAccountState> {

    state = {
        accountName: undefined 
    } as AddAccountState;

    handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            ...this.state,
            [event.target.name]: event.target.value
        })
    }

    handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        if(this.state.accountName) {
            this.props.onAddAccount(this.state.accountName)
        }
    }

    render() {

        const {classes} = this.props;
        return (
            <main>
            <Typography>
                Add account for your child
            </Typography>
            <form className={classes.form} onSubmit={this.handleSubmit}>
                <TextField 
                    name="accountName" 
                    id='accountName' 
                    label="Child's Name"
                    fullWidth={true}
                    onChange={this.handleChange} 
                    value={this.state.accountName || ''} 
                     />
                <Button
                    className={classes.submit}
                    type="submit"
                    color="primary"
                    variant="raised" 
                    name="Save">
                    Save
                </Button>
            </form>
            </main>
        )

    }
}

export const AddAccount = withStyles(styles)(AddAccountBase)


