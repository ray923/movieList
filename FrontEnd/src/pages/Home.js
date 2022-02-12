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
import CustomInput from 'components/CustomInput/CustomInput.js';
import Button from "components/CustomButtons/Button.js";
import Card from './HomeComponent/Card';

import { HTMLDecode } from "../utils/helper.js";
import {useTranslation} from 'react-i18next';


import styles from "assets/jss/material-kit-react/views/components.js";
import search_styles from "assets/jss/material-kit-react/views/componentsSections/navbarsStyle.js";
import movies from "data/movie.json";

const useStyles = makeStyles(styles);
const useSearch_Styles = makeStyles(search_styles);

export default function Home(props) {
  const { t } = useTranslation();
  const classes = useStyles();
  const search_classes = useSearch_Styles();
  const { ...rest } = props;
  const [loadedMoives, setLoadedMoives] = useState([]);
  const [searchInput, setSearchInput] = useState("");
  const numberPerLoad = 30;

  const fetchMoreData = () => {
    var startIndex = loadedMoives.length;
    var endIndex = startIndex + numberPerLoad;
    var loadedMovieList = movies.slice(startIndex, endIndex);
    setLoadedMoives([...loadedMoives, ...loadedMovieList]);
  }

  useEffect(() => { 
    setSearchInput("");
    fetchMoreData();
  }, []);// eslint-disable-line react-hooks/exhaustive-deps

  var searchMovie = () => { 
    console.log(searchInput);
    if (searchInput.length > 0) {
      var searchResult = movies.filter((movie) => {
        return ((movie.Name && movie.Name.toLowerCase().includes(searchInput.toLowerCase())));
          //|| (movie.SubTitle && movie.SubTitle.toLowerCase().includes(searchInput.toLowerCase()))
          //|| (movie.Introduction && movie.Introduction.toLowerCase().includes(searchInput.toLowerCase()))
          //|| (movie.Overview && movie.Overview.toLowerCase().includes(searchInput.toLowerCase()));
      });
      setLoadedMoives([...searchResult]);
    } else { 
      var loadedMovieList = movies.slice(0, numberPerLoad);
      setLoadedMoives([...loadedMovieList]);
    }
  }

  function RenderMovieCards() {
    return loadedMoives.map((item) => {
      return (
        <GridItem xs={2} className={classNames(classes.cardMargin)} key={item.Id}>
            <Card
              Id={item.Id}
              Name={HTMLDecode(item.Name)}
              ListImgUrl={item.ListImgUrl}
              SubTitle={item.SubTitle}
            ></Card>
        </GridItem>
      )
    })
  };

  function renderSearchBar() {
    return (
    <div style={{ "mixBlendMode": "difference"}}>
      <CustomInput
          white
          inputRootCustomClasses={search_classes.inputRootCustomClasses}
          formControlProps={{
            className: search_classes.formControl,
          }}
          inputProps={{
            placeholder: t('Search'),
            inputProps: {
              "aria-label": "Search",
              className: search_classes.searchInput,
            },
          }}
          onChangeValue={(e) => setSearchInput(e)}
          searchValue={searchInput}
      />
      <Button justIcon round color="white" onClick = {() => searchMovie()}>
        <Search className={search_classes.searchIcon} />
      </Button>
    </div>)
  }

  return (
    <div>
      <Header
        brand={t('SiteName')}
        searchBar={renderSearchBar()}
        rightLinks={<HeaderLinks />}
        fixed
        color="transparent"
        changeColorOnScroll={{
          height: 400,
          color: "white"
        }}
        {...rest}
      />
      <Parallax image={require("assets/img/profile-bg.jpg")}>
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
          next={fetchMoreData}
          hasMore={searchInput.length > 0 ? false : movies.length > loadedMoives.length ? true : false}
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
