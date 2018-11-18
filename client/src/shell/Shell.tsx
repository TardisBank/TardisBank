import * as React from 'react';
import { WithStyles, withStyles, Button } from '@material-ui/core';
import { Header } from './components/header/Header';
import { AccountList } from '../account/AccountList'
import { Content, ContentView } from './components/content/Content';
import { styles } from './Shell.styles';
import { Navigation } from './components/navigation/Navigation';
import { AddCircle } from '@material-ui/icons';

type ShellProps = WithStyles<typeof styles>;

type ShellState = {
    isSideBarOpen: boolean,
    contentView: ContentView,
    selectedAccount?: string
};

class shell extends React.Component<ShellProps, ShellState> {

    state = { isSideBarOpen: false, selectedAccount: undefined, contentView: ContentView.Empty }

    onSidebarClick = () => {
        this.setState(state => {
            return {
                ...state,
                isSideBarOpen: !state.isSideBarOpen
            }
        });
    }

    onMenuClick = (selectedAccount: string) => {
        this.setState(state => {
            return {
                ...state,
                contentView: ContentView.ShowAccount,
                selectedAccount
            }
        })
    }

    onAddAccont = () => {
        this.setState(state => {
            return {
                ...state,
                contentView: ContentView.AddAccount
            }
        })
    }

    render() {
        const { classes } = this.props;
        return (
            <div className={classes.root}>
                <Header isOpen={this.state.isSideBarOpen} onMenuClick={this.onSidebarClick} />
                <Navigation
                    isOpen={this.state.isSideBarOpen}
                    onAddAccount={this.onAddAccont}
                    onToobarIconClick={this.onSidebarClick} >
                    <>
                        <AccountList
                            accounts={[{ id: '1', name: 'Child A' }]}
                            selectedAccountId={this.state.selectedAccount}
                            onAccountSelected={this.onMenuClick} />
                        <Button
                            onClick={this.onAddAccont}>
                            <AddCircle fontSize="large" />
                        </Button>
                    </>
                </Navigation>
                <main className={classes.content}>
                    <div className={classes.appBarSpacer} />
                    <Content
                        selectedAccount={this.state.selectedAccount}
                        viewType={this.state.contentView} />
                </main>
            </div>);

    }
}



export const Shell = withStyles(styles)(shell);