import React from "react";
import ReactDOM from "react-dom";
import { Router, Switch, Route } from "react-router-dom";
import { createBrowserHistory } from "history";
import './i18n';
import "assets/scss/material-kit-react.scss?v=1.9.0";

import Home from "pages/Home.js";
import Cate from "pages/Category.js";
import Download from "pages/Download.js";
import Content from "pages/Content.js";

var hist = createBrowserHistory();

ReactDOM.render(
  <Router history={hist}>
    <Switch>
      <Route exact path='/' component={Home} />
      <Route exact path='/:pageNumber' component={Home} />
      <Route exact path='/Cat/:catId/:pageNumber' component={Cate} />
      <Route exact path='/D/:movieId' component={Download} />
      <Route exact path='/content/:movieId' component={Content} />
    </Switch>
  </Router>,
  document.getElementById("root")
);
