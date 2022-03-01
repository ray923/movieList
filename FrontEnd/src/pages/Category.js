import React from "react";
import { useState, useEffect } from "react"; // useState is a hook
import InfiniteScroll from 'react-infinite-scroll-component';
// nodejs library that concatenates classes
import classNames from "classnames";
// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
import Search from "@material-ui/icons/Search";
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

import { HTMLDecode } from "../utils/helper.js";
import { useTranslation } from 'react-i18next';

import styles from "assets/jss/material-kit-react/views/components.js";
import { getCategory } from "../actions/index.js";

const useStyles = makeStyles(styles);

export default function Category(props) {
  const { t } = useTranslation();
  const classes = useStyles();
  const { ...rest } = props;
  const [loadedMoives, setLoadedMoives] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [hasMore, setHasMore] = useState(true);

  const fetchMoreData = async (init) => {
    var loadedMovieList = await getCategory(props.location.pathname.split('/')[2], init? 1 : pageNumber);
    if (loadedMovieList.length === 0 || loadedMovieList.length < 48) {
      setHasMore(false);
    }
    else
    {
      setHasMore(true);
    }
    console.log(pageNumber);
    setLoadedMoives((pre) => { return [...pre, ...loadedMovieList] });
    if (!init) {
      setPageNumber(pageNumber + 1);
    } else {
      setPageNumber(2);
    }
  }

  useEffect(() => { 
    setLoadedMoives([]);
    setPageNumber(1);
    fetchMoreData(true);
  }, [props.location.pathname]);// eslint-disable-line react-hooks/exhaustive-deps

  function RenderMovieCards() {
    return loadedMoives.map((item) => {
      return (
        <GridItem xs={2} className={classNames(classes.cardMargin)} key={item.id}>
            <Card
              Id={item.id}
              Name={HTMLDecode(item.name)}
              ListImgUrl={item.listImgUrl}
              SubTitle={item.subTitle}
            ></Card>
        </GridItem>
      )
    })
  };

  return (
    <div>
      <Header
        brand={t('SiteName')}
        searchBar={<></>}
        rightLinks={<HeaderLinks />}
        fixed
        color="transparent"
        changeColorOnScroll={{
          height: 400,
          color: "white"
        }}
        {...rest}
      />
      <Parallax image={require("assets/img/landing-bg.jpg")}>
        <div className={classes.container}>
          <GridContainer>
            <GridItem>
              <div className={classes.brand}>
                <h1 className={classes.title}></h1>
                <h3 className={classes.subtitle}></h3>
              </div>
            </GridItem>
          </GridContainer>
        </div>
      </Parallax>
      <div className={classNames(classes.main, classes.mainRaised)}>
        <InfiniteScroll
          dataLength={loadedMoives.length}
          next={() => fetchMoreData(false)}
          hasMore={hasMore}
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
