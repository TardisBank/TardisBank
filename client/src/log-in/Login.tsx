import * as React from 'react';
import { Avatar, withStyles, WithStyles, Typography, Button, TextField } from '@material-ui/core';
import { LockOutlined } from '@material-ui/icons';
import Paper from '@material-ui/core/Paper';
import { styles } from './Login.styles';
import { Form } from 'src/controls';

type LoginState = {
    email: string,
    password: string
};

export enum LoginProcess {
    Ready,
    Loading,
    Error
}

type LoginStateProps = {
    loginProcess: LoginProcess
}

type LoginDispatchProps = {
    onLogin: (email: string, password: string) => void;
    toggleSignIn: () => void;
}

type LoginProps = LoginDispatchProps & LoginStateProps & WithStyles<typeof styles>;

class LoginBase extends React.Component<LoginProps, LoginState> {

    state = {
        email: '',
        password: ''
    } as LoginState;

    handleSubmit = (event: React.FormEvent) => {
        const { email, password } = this.state;
        this.props.onLogin(email, password);
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
                            <LockOutlined />
                        </Avatar>
                        <Typography variant="headline">
                            Sign in
                        </Typography>
                        {this.props.loginProcess === LoginProcess.Error &&
                            <Typography variant="subheading">
                                Incorrect email or password
                            </Typography>
                        }
                        <Form onSubmit={this.handleSubmit}
                        submit={
                           <Button
                                type="submit"
                                fullWidth={true}
                                variant="raised"
                                color="primary"
                                disabled={this.props.loginProcess === LoginProcess.Loading}
                            >
                                Sign in
                            </Button>
                        }>
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
                         </Form>
                        <Typography 
                            variant="caption">
                            <p>New to Tardis Bank?
                            <Button
                                    color="default"
                                    variant="outlined"
                                    fullWidth={true}
                                    className={classes.button}
                                    onClick={this.props.toggleSignIn}
                                >
                                    Register Here</Button>
                            </p>

                        </Typography>
                    </Paper>
                </main>
            </>
        );
    }

}

export const Login = withStyles(styles)(LoginBase);