import React from "react";
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
  const movieId = props.location.pathname.split('/')[2];
  const movie = Movies.find((item)=> item.Id.toString()===movieId);
  
  const LeftLinks = <Tooltip
    title="返回首页"
    placement="left"
    classes={{ tooltip: classes.tooltip }}
  >
    <Button
      color="transparent"
      href="/"
      className={classes.navLink}
    >
      <i className={classes.socialIcons + " fas fa-arrow-left"} /> Back
    </Button>
  </Tooltip>;

  return (
    <div>
      <Header
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
          {/* <Button color="primary" href={`../download/` + movie.Id}>Go to Downlaod</Button> */}
          <Button target="_blank" color="primary" href={movie.DownloadUrl}>Go to Downlaod</Button>
        </div>
      </div>
      <Footer />
    </div>
  );
}
