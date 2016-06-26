﻿import * as React from "react";
import * as ReactDOM from "react-dom";

interface IHomeProps {
    pageTitle: string;
}

class Home extends React.Component<IHomeProps, {}> {
    render() {
        return <div>{this.props.pageTitle}.If you can see this message means you have successfully configured reactjs+typescript+webpack+babel-loader.</div>;
    }
}

ReactDOM.render(
    <Home pageTitle="Dashboard" />,
    document.getElementById("home")
);

export default Home;