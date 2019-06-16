import * as React from "react";
import { WithStyles, withStyles, Button } from "@material-ui/core";
import { Header } from "./components/header/Header";
import { Content, ContentView } from "./components/content/Content";
import { styles } from "./Shell.styles";
import { Navigation } from "./components/navigation/Navigation";
import { AddCircle } from "@material-ui/icons";
import { AccountsContainer } from "../account/AccountsContainer";
import { Account } from "../model";

type ShellProps = WithStyles<typeof styles>;

type ShellState = {
  isSideBarOpen: boolean;
  contentView: ContentView;
  selectedAccount?: string;
  accountList: ReadonlyArray<Account>;
};

class shell extends React.Component<ShellProps, ShellState> {
  state = {
    isSideBarOpen: false,
    selectedAccount: undefined,
    contentView: ContentView.Empty,
    accountList: []
  };

  onSidebarClick = () => {
    this.setState(state => {
      return {
        ...state,
        isSideBarOpen: !state.isSideBarOpen
      };
    });
  };

  onMenuClick = (selectedAccount: string) => {
    this.setState(state => {
      return {
        ...state,
        contentView: ContentView.ShowTransactions,
        selectedAccount
      };
    });
  };

  onAddAccont = () => {
    this.setState(state => {
      return {
        ...state,
        contentView: ContentView.AddAccount
      };
    });
  };

  onAccountsLoaded = (accountList: ReadonlyArray<Account>) => {
    this.setState(state => {
      return {
        ...state,
        accountList
      };
    });
  };

  onAccountAdded = (account: Account) => {
    this.setState(state => {
      return {
        ...state,
        selectedAccount: account.id,
        contentView: ContentView.ShowTransactions,
        accountList: [...state.accountList, account]
      };
    });
  };

  getActiveAccount = () => {
    return (
      this.state.selectedAccount &&
      this.state.accountList.find(
        (x: Account) => x.id === this.state.selectedAccount
      )
    );
  };

  render() {
    const { classes } = this.props;
    return (
      <div className={classes.root}>
        <Header
          isOpen={this.state.isSideBarOpen}
          onMenuClick={this.onSidebarClick}
        />
        <Navigation
          isOpen={this.state.isSideBarOpen}
          onAddAccount={this.onAddAccont}
          onToobarIconClick={this.onSidebarClick}
        >
          <>
            <AccountsContainer
              selectedAccountId={this.state.selectedAccount}
              onAccountSelected={this.onMenuClick}
              onAccountsLoaded={this.onAccountsLoaded}
              accounts={this.state.accountList}
            />
            <Button onClick={this.onAddAccont}>
              <AddCircle fontSize="large" />
            </Button>
          </>
        </Navigation>
        <main className={classes.content}>
          <div className={classes.appBarSpacer} />
          <Content
            selectedAccount={this.state.selectedAccount}
            activeAccount={this.getActiveAccount()}
            onAccountAdded={this.onAccountAdded}
            viewType={this.state.contentView}
          />
        </main>
      </div>
    );
  }
}

export const Shell = withStyles(styles)(shell);
