import React from "react";
import { Route, useLocation } from "react-router-dom";
import { useTransition, animated } from 'react-spring';
// pages for this product
import Content from "pages/Content.js";

export default function App() {
  const location = useLocation()
  const transitions = useTransition(location, {})

  return transitions((style, { item: location, props, key }) => (
        <animated.div key={key} style={style}>
          <Route exact path='/content/:movieId' component={Content} />
        </animated.div>))
}
