import React from "react";
import ReactDOM from "react-dom";
import {Route, HashRouter } from "react-router-dom";
import "assets/scss/material-kit-react.scss?v=1.9.0";

// pages for this product
// import Components from "views/Components/Components.js";
// import LandingPage from "views/LandingPage/LandingPage.js";
// import ProfilePage from "views/ProfilePage/ProfilePage.js";
// import LoginPage from "views/LoginPage/LoginPage.js";
import Home from "pages/Home.js";
import Cate from "pages/Category.js";
import APP from "./APP.js";

ReactDOM.render(
  // <Router history={hist}>
  //   <Switch>
  //     {/* <Route path="/landing-page" component={LandingPage} />
  //     <Route path="/profile-page" component={ProfilePage} />
  //     <Route path="/login-page" component={LoginPage} /> */}
  //     <Route path="/content" component={Content}/>
  //     <Route path="/download" component={Dowlaod} />
  //     <Route path="/" component={Home} exact/>
  //     {/* <Route path="/" component={Components} /> */}
  //   </Switch>
  // </Router>,
  <HashRouter>
    <Route exact path='/' component={Home} />
    <Route exact path='/Cat/:catId' component={Cate} />
    <APP/>
  </HashRouter>,
  document.getElementById("root")
);
