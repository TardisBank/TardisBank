import * as React from 'react';
import { Avatar, withStyles, WithStyles, Typography, FormControl, InputLabel, Input, Button } from '@material-ui/core';
import { LockOutlined } from '@material-ui/icons';
import Paper from '@material-ui/core/Paper';
import { styles } from './Login.styles';
import { getMessagingClient, LoginRequest } from '../messaging/messagingClient';
import { LoginResultDto } from 'tardis-bank-dtos'

type LoginState = {
    email: string,
    password: string,
    isLoding: boolean,
    success: boolean
};

type LoginDispatchProps = {
    onSubmit?: (state: LoginState) => void;
    onAuthenticated: (token: string) => void;
}

type LoginProps = LoginDispatchProps & WithStyles<typeof styles>;

class LoginBase extends React.Component<LoginProps, LoginState> {

    state = {
        email: '',
        password: '',
        isLoding: false,
        success: true
    } as LoginState;

    constructor(props: LoginProps) {
        super(props);

        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
    }

    handleSubmit(event: React.FormEvent) {
        const { email, password } = this.state;

        this.setState({ ...this.state, isLoding: true });
        event.preventDefault();

        getMessagingClient().post<LoginRequest, LoginResultDto>('api/login', { email, password })
            .then(response => {
                this.props.onAuthenticated(response.Token);
            }).catch(() => {
                this.setState({
                    ...this.state,
                    isLoding: false,
                    success: false
                });
            })

    }

    handleChange(event: React.ChangeEvent<HTMLInputElement>) {
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
                        {this.state.success === false &&
                            <Typography variant="subheading">
                                Incorrect email or password
                            </Typography>
                        }
                        <form className={classes.form} onSubmit={this.handleSubmit}>
                            <FormControl margin="normal" required={true} fullWidth={true}>
                                <InputLabel htmlFor="email">Email</InputLabel>
                                <Input id="email"
                                    name="email"
                                    autoComplete="email"
                                    autoFocus={true}
                                    value={this.state.email}
                                    onChange={this.handleChange} />
                            </FormControl>
                            <FormControl margin="normal" required={true} fullWidth={true}>
                                <InputLabel htmlFor="password">Password</InputLabel>
                                <Input
                                    onChange={this.handleChange}
                                    id="passowrd"
                                    name="password"
                                    type="password"
                                    value={this.state.password}
                                    autoComplete="current-password" />
                            </FormControl>
                            <Button
                                type="submit"
                                fullWidth={true}
                                variant="raised"
                                color="primary"
                                disabled={this.state.isLoding}
                                className={classes.submit}
                            >
                                Sign in
                            </Button>
                        </form>
                    </Paper>
                </main>
            </>
        );
    }

}

export const Login = withStyles(styles)(LoginBase);