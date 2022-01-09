import React from "react";
import { useState, useEffect } from "react"; // useState is a hook
// nodejs library that concatenates classes
import classNames from "classnames";
// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
import Tooltip from "@material-ui/core/Tooltip";

// @material-ui/icons

// core components
import Header from "components/Header/Header.js";
import Footer from "components/Footer/Footer.js";
import Button from "components/CustomButtons/Button.js";
import HeaderLinks from "components/Header/HeaderLinks.js";
import Parallax from "components/Parallax/Parallax.js";

import styles from "assets/jss/material-kit-react/views/landingPage.js";

// Sections for this page
import ContentSection from "./Sections/ContentSection.js";

import Movies from "data/movie.json";

//const dashboardRoutes = ["/Home"];

const useStyles = makeStyles(styles);

export default function Content(props) {
  const classes = useStyles();
  const { ...rest } = props;
  const [movie,setMovie] = useState(null);
  
  useEffect(() => {
      const movieId = props.location.pathname.split('/')[2];
      setMovie(Movies.find((item)=> item.Id.toString()===movieId));
    }, [props.location.pathname]);// eslint-disable-line react-hooks/exhaustive-deps

  const LeftLinks = <Tooltip
    title="返回首页"
    placement="left"
    classes={{ tooltip: classes.tooltip }}
  >
    <Button
      color="transparent"
      href="/#/"
      className={classes.navLink}
    >
      <i className={classes.socialIcons + " fas fa-arrow-left"} /> Back
    </Button>
  </Tooltip>;

  return (
    <div>
      {movie && (<><Header
        color="transparent"
        //routes={dashboardRoutes}
        brand={movie.Name}
        rightLinks={<HeaderLinks />}
        leftLinks={LeftLinks}
        fixed
        changeColorOnScroll={{
          height: 400,
          color: "white"
        }}
        {...rest}
      />
      <Parallax filter image={movie.ImgUrl}>
        {/* <div className={classes.container}>
          <GridContainer>
            <GridItem xs={12} sm={12} md={6}>
              <h1 className={classes.title}>{movie.Name}</h1>
            </GridItem>
          </GridContainer>
        </div> */}
      </Parallax>
      <div className={classNames(classes.main, classes.mainRaised)}>
        <div className={classes.container}>
          <ContentSection 
            Movie={movie}
          />
          <Button target="_blank" color="primary" href={`/#/D/` + movie.Id}>Go to Downlaod</Button>
          <Button target="_blank" color="primary" href={movie.DownloadUrl}>Go to Downlaod</Button>
        </div>
      </div>
      <Footer /></>)}
    </div>
  );
}
