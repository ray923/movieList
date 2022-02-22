import React from "react";
import { useState, useEffect } from "react"; // useState is a hook
// nodejs library that concatenates classes
import classNames from "classnames";
// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
// core components
import Header from "components/Header/Header.js";
import Footer from "components/Footer/Footer.js";
import GridContainer from "components/Grid/GridContainer.js";
import GridItem from "components/Grid/GridItem.js";
import HeaderLinks from "components/Header/HeaderLinks.js";
import Parallax from "components/Parallax/Parallax.js";

import styles from "assets/jss/material-kit-react/views/landingPage.js";

// Sections for this page
import DownloadSection from "./Sections/DownloadSection.js";
import { getMovieDownload } from '../actions/index.js';

const dashboardRoutes = [];

const useStyles = makeStyles(styles);

export default function Downlaod(props) {
  const classes = useStyles();
  const { ...rest } = props;
  const [movie, setMovie] = useState([]);

  useEffect(() => { 
    async function getDownload(id) {
      const res = await getMovieDownload(id);
      setMovie(res);
    }
    const movieId = props.location.pathname.split('/')[2];
    getDownload(movieId);
  },[props.location.pathname]);// eslint-disable-line react-hooks/exhaustive-deps

  return (
    <div>
      <Header
        color="transparent"
        routes={dashboardRoutes}
        brand= {movie.movieName + ' Download'}
        rightLinks={<HeaderLinks />}
        fixed
        changeColorOnScroll={{
          height: 400,
          color: "white"
        }}
        {...rest}
      />
      <Parallax filter image={require("assets/img/landing-bg.jpg")} className={classNames(classes.parallaxDownload)}>
        <div className={classes.container}>
          <GridContainer>
            <GridItem xs={12} sm={12} md={6}>
              <h4></h4>
            </GridItem>
          </GridContainer>
        </div>
      </Parallax>
      <div className={classNames(classes.main, classes.mainRaised)}>
        <div className={classes.container}>
          <DownloadSection 
            Download = {movie}
          />
        </div>
      </div>
      <Footer />
    </div>
  );
}
