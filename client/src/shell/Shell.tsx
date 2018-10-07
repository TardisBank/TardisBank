import * as React from 'react';
import { WithStyles, withStyles, createStyles, Theme } from '@material-ui/core';
import { Header } from './components/header/Header';
import { Navigation } from './components/navigation/Navigation'

const styles = (theme: Theme) => createStyles({
    root: {
        display: 'flex',
    },
    content: {
        flexGrow: 1,
        padding: theme.spacing.unit * 3,
        height: '100vh',
        overflow: 'auto'
    },
    appBarSpacer: theme.mixins.toolbar
});

type ShellProps = WithStyles<typeof styles>;

type ShellState = {
    isSideBarOpen: boolean
};

class shell extends React.Component<ShellProps, ShellState> {

    state = { isSideBarOpen: false }

    constructor(props: ShellProps) {
        super(props);

        this.handleSidebarClick = this.handleSidebarClick.bind(this);
    }


    handleSidebarClick() {
        this.setState(state => {return {isSideBarOpen: !state.isSideBarOpen}});
    }

    render() {
        const { classes } = this.props;
        return (
            <div className={classes.root}>
                <Header isOpen={this.state.isSideBarOpen} onMenuClick={this.handleSidebarClick} />
                <Navigation isOpen={this.state.isSideBarOpen} onToobarIconClick={this.handleSidebarClick} />
                <main className={classes.content}>
                    <div className={classes.appBarSpacer} />
                    <p>Stuff</p>
                </main>
            </div>);

    }
}

export const Shell = withStyles(styles)(shell);