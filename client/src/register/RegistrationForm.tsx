import * as React from 'react';
import { Avatar, withStyles, WithStyles, Typography, Button, TextField } from '@material-ui/core';
import { PersonAdd } from '@material-ui/icons';
import Paper from '@material-ui/core/Paper';
import { styles } from './RegistrationForm.styles';

type RegisterState = {
    email: string,
    password: string,
    confirmPassword: string,
    passowrdsMismatched: boolean
};

export enum RegisterProcess {
    Ready,
    Loading,
    Error,
    Sent
}

type RegisterStateProps = {
    registerProcess: RegisterProcess,
}

type RegisterDispatchProps = {
    onRegister: (email: string, password: string, confirmPassword: string) => void;
    toggleSignIn: () => void;
}

type RegisterProps = RegisterDispatchProps & RegisterStateProps & WithStyles<typeof styles>;

class RegisterBase extends React.Component<RegisterProps, RegisterState> {

    state = {
        email: '',
        password: '',
        confirmPassword: '',
        passowrdsMismatched: false
    } as RegisterState;

    handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        const { email, password, confirmPassword } = this.state;
        if (password !== confirmPassword) {
            this.setState(state => {
                return {
                    ...state,
                    passowrdsMismatched: true
                }
            });
        }
        else {
            this.setState(state => {
                return {
                    ...state,
                    passowrdsMismatched: false
                }
            });
            this.props.onRegister(email, password, confirmPassword);
        }
    }

    handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            ...this.state,
            [event.target.name]: event.target.value
        })
    }

    render() {
        const { classes } = this.props;

        return (
            <>
                <main className={classes.layout}>
                    <Paper className={classes.paper}>
                        <Avatar className={classes.avatar}>
                            <PersonAdd />
                        </Avatar>
                        <Typography variant="headline">
                            Register
                        </Typography>
                        {this.props.registerProcess === RegisterProcess.Error &&
                            <Typography variant="subheading">
                                Incorrect email or password
                            </Typography>
                        }
                        {this.state.passowrdsMismatched &&
                            <Typography variant="subheading">
                                The passwords do not match
                            </Typography>
                        }
                        <form className={classes.form} onSubmit={this.handleSubmit}>
                            <TextField
                                id="email"
                                name="email"
                                label="Email"
                                fullWidth={true}
                                autoFocus={true}
                                required={true}
                                margin="normal"
                                value={this.state.email}
                                onChange={this.handleChange}
                                variant={"outlined"}
                            />
                            <TextField
                                id="password"
                                name="password"
                                label="Password"
                                type="password"
                                fullWidth={true}
                                required={true}
                                margin="normal"
                                value={this.state.password}
                                onChange={this.handleChange}
                                variant={"outlined"}

                            />
                            <TextField
                                id="confirm-password"
                                name="confirmPassword"
                                label="Confirm Password"
                                type="password"
                                fullWidth={true}
                                required={true}
                                margin="normal"
                                value={this.state.confirmPassword}
                                onChange={this.handleChange}
                                variant={"outlined"}

                            />
                            <Button
                                type="submit"
                                fullWidth={true}
                                variant="raised"
                                color="primary"
                                disabled={this.props.registerProcess === RegisterProcess.Loading}
                                className={classes.submit}
                            >
                                Register
                            </Button>
                        </form>
                        <Typography variant="caption">
                            <a href="#" onClick={this.props.toggleSignIn}>Sign In</a>
                        </Typography>
                    </Paper>
                </main>
            </>
        );
    }

}

export const Register = withStyles(styles)(RegisterBase);