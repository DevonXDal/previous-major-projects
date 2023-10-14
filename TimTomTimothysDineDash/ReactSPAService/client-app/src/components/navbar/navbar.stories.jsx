import React from 'react';

import Navbar from './navbar.component';
import { BrowserRouter } from 'react-router-dom';

export default {
    title: 'Components/Navbar',
    component: Navbar,
    tags: ['autodocs'],
};

const Template = args => <BrowserRouter><Navbar /></BrowserRouter>;

export const Static = Template.bind({});

