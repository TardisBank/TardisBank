import * as React from 'react';
import { Account } from '../../../model';
import { AddAccountContainer } from '../../../account/';
import { Paper, WithStyles, withStyles } from '@material-ui/core';
import { styles } from './Content.styles';

export enum ContentView {
    'ShowAccount',
    'AddAccount',
    'Empty'
}

type ContentProps = {
    viewType: ContentView,
    selectedAccount?: string,
    onAccountAdded: (account: Account) => void;
} & WithStyles<typeof styles>;

const GetContent: React.SFC<ContentProps> = (props: ContentProps) => {
    switch (props.viewType) {
        case ContentView.AddAccount:
            return <AddAccountContainer {...props} />
        case ContentView.ShowAccount:
            return <div>An account with id {props.selectedAccount}</div>
        case ContentView.Empty:
            return <div>Nothing to see</div>
        default:
            return <div>Unknown ContentView</div>
    }
}

class ContentBase extends React.Component<ContentProps> {

    render() {
        const { classes } = this.props;
        return (
            <Paper className={classes.paper}>
                <GetContent {...this.props} />
            </Paper>
        )
    }
}

export const Content = withStyles(styles)(ContentBase);