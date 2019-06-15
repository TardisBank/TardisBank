import * as React from "react";
import { Account } from "../../../model";
import { AddAccountContainer } from "../../../account/";
import { Paper, WithStyles, withStyles } from "@material-ui/core";
import { styles } from "./Content.styles";
import { ListTransactionContainer } from "../../../transaction/ListTransactionsContainer";

export enum ContentView {
  "AddAccount",
  "ShowTransactions",
  "Empty"
}

type ContentProps = {
  viewType: ContentView;
  selectedAccount?: string;
  onAccountAdded: (account: Account) => void;
  activeAccount?: Account;
} & WithStyles<typeof styles>;

const GetContent: React.SFC<ContentProps> = (props: ContentProps) => {
  switch (props.viewType) {
    case ContentView.AddAccount:
      return <AddAccountContainer {...props} />;
    case ContentView.ShowTransactions:
      return props.activeAccount ? (
        <ListTransactionContainer account={props.activeAccount} />
      ) : (
        <div>No such account</div>
      );
    case ContentView.Empty:
      return <div>Nothing to see</div>;
    default:
      return <div>Unknown ContentView</div>;
  }
};

class ContentBase extends React.Component<ContentProps> {
  render() {
    const { classes } = this.props;
    return (
      <Paper className={classes.paper}>
        <GetContent {...this.props} />
      </Paper>
    );
  }
}

export const Content = withStyles(styles)(ContentBase);
