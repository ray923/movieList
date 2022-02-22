import React from "react";
import { useState, useEffect } from "react"; // useState is a hook
// nodejs library that concatenates classes
import classNames from "classnames";
// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
import Tooltip from "@material-ui/core/Tooltip";
import {useTranslation} from 'react-i18next';
import { HTMLDecode } from "../utils/helper.js";
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
import { getMovieDetail } from '../actions/index.js';


const useStyles = makeStyles(styles);

export default function Content(props) {
  const { t } = useTranslation();
  const classes = useStyles();
  const { ...rest } = props;
  const [movie,setMovie] = useState(null);
  
  useEffect(() => {
      async function getMovie(id) {
        const res = await getMovieDetail(id);
        console.log(res);
        setMovie(res);
      }
      const movieId = props.location.pathname.split('/')[2];
      getMovie(movieId);
    }, [props.location.pathname]);// eslint-disable-line react-hooks/exhaustive-deps

  const LeftLinks = <Tooltip
    title={t('Back')}
    placement="left"
    classes={{ tooltip: classes.tooltip }}
  >
    <Button
      color="transparent"
      href="/"
      className={classes.navLink}
    >
      <i className={classes.socialIcons + " fas fa-arrow-left"} /> {t('Back')}
    </Button>
  </Tooltip>;

  return (
    <div>
      {movie && (<><Header
        color="transparent"
        //routes={dashboardRoutes}
        brand={HTMLDecode(movie.name)}
        rightLinks={<HeaderLinks />}
        leftLinks={LeftLinks}
        fixed
        changeColorOnScroll={{
          height: 400,
          color: "white"
        }}
        {...rest}
      />
      <Parallax filter image={movie.imgUrl}>
      </Parallax>
      <div className={classNames(classes.main, classes.mainRaised)}>
        <div className={classes.container}>
          <ContentSection 
            Movie={movie}
          />
          {/* <Button target="_blank" color="primary" href={`/D/` + movie.Id}>Go to Downlaod</Button> */}
          <Button target="_blank" color="primary" href={movie.downloadUrl}>Go to Downlaod</Button>
        </div>
      </div>
      <Footer /></>)}
    </div>
  );
}
