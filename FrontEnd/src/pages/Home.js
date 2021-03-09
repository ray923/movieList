import React from "react";
// nodejs library that concatenates classes
import classNames from "classnames";
// react components for routing our app without refresh
import { Link } from "react-router-dom";
// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
// @material-ui/icons
// core components
import Header from "components/Header/Header.js";
import Footer from "components/Footer/Footer.js";
import GridContainer from "components/Grid/GridContainer.js";
import GridItem from "components/Grid/GridItem.js";
import Parallax from "components/Parallax/Parallax.js";
// sections for this page
import HeaderLinks from "components/Header/HeaderLinks.js";
import Card from './HomeComponent/Card';

import styles from "assets/jss/material-kit-react/views/components.js";
import movies from "data/movie.json";

const useStyles = makeStyles(styles);

export default function Home(props) {
  const classes = useStyles();
  const { ...rest } = props;

  function RenderMovieCards () {
    return movies.movies.map((item) => {
      return (
        <GridItem spacing={2} xs={2}>
            <Card
              Id={item.Id}
              Name={item.Name}
              ListImgUrl={item.ListImgUrl}
              SubTitle={item.SubTitle}
            ></Card>
        </GridItem>
      )
    })
  };

  return (
    <div>
      <Header
        brand="Movie List Demo"
        rightLinks={<HeaderLinks />}
        fixed
        color="transparent"
        changeColorOnScroll={{
          height: 400,
          color: "white"
        }}
        {...rest}
      />
      <Parallax image={require("assets/img/bg4.jpg")}>
        <div className={classes.container}>
          <GridContainer>
            <GridItem>
              <div className={classes.brand}>
                <h1 className={classes.title}>Ads.</h1>
                <h3 className={classes.subtitle}>
                  here is an Ads.
                </h3>
              </div>
            </GridItem>
          </GridContainer>
        </div>
      </Parallax>
      <div className={classNames(classes.main, classes.mainRaised)}>
      <GridContainer>
        {RenderMovieCards()}
      </GridContainer>
      </div>
      <Footer />
    </div>
  );
}
