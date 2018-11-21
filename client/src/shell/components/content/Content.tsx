import * as React from 'react';
import { AddAccountContainer } from '../../../account/';

export enum ContentView {
    'ShowAccount',
    'AddAccount',
    'Empty'
}

type ContentProps = {
    viewType: ContentView,
    selectedAccount?: string
}

export const Content: React.SFC<ContentProps> = (props: ContentProps) => {
    switch (props.viewType) {
        case ContentView.AddAccount:
            return <AddAccountContainer />
        case ContentView.ShowAccount:
            return <div>An account with id {props.selectedAccount}</div>
        case ContentView.Empty:
            return <div>Nothing to see</div>
        default:
            return <div>Unknown ContentView</div>
    }
}