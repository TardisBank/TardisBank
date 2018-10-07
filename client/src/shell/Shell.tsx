import * as React from 'react';
import { Header } from './components/header/Header'; 
import { Navigation } from './components/navigation/Navigation'

export const Shell : React.SFC<{}> = () => 
    <>
    <Header />
    <Navigation />
    </>
