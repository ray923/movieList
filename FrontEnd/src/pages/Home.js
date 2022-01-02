import React from "react";
import { useState, useEffect } from "react"; // useState is a hook
import InfiniteScroll from 'react-infinite-scroll-component';
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
  const [loadedMoives, setLoadedMoives] = useState([]);
  const numberPerLoad = 30;

  useEffect(() => { 
    fetchMoreData();
  }, []);
  const fetchMoreData = () => {
    console.log("fetchMoreData");
    var startIndex = loadedMoives.length;
    console.log("startIndex: " + startIndex);
    var endIndex = startIndex + numberPerLoad;
    console.log("movies: " + movies);
    var loadedMovieList = movies.slice(startIndex, endIndex);
    setLoadedMoives([...loadedMoives, ...loadedMovieList]);
    console.log(loadedMoives);
  }

  function RenderMovieCards() {
    console.log("RenderMovieCards");
    return loadedMoives.map((item) => {
      return (
        <GridItem xs={2} className={classNames(classes.cardMargin)} key={item.Id}>
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
        <InfiniteScroll
          dataLength={loadedMoives.length}
          next={fetchMoreData}
          hasMore={movies.length > loadedMoives.length ? true : false}
          loader={<div style={{textAlign:"center"}}><h4>Loading...</h4></div>}
          endMessage={<div style={{textAlign:"center"}}><h4>End</h4></div>}
        >
          <GridContainer>
            {RenderMovieCards()}
          </GridContainer>
        </InfiniteScroll>
      </div>
      <Footer />
    </div>
  );
}
