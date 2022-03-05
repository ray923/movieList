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
import { getAllMovies, search } from "../actions/index.js";

const useStyles = makeStyles(styles);
const useSearch_Styles = makeStyles(search_styles);

export default function Home(props) {
  const { t } = useTranslation();
  const classes = useStyles();
  const search_classes = useSearch_Styles();
  const { ...rest } = props;
  const [loadedMoives, setLoadedMoives] = useState([]);
  const [pageNumber, setPageNumber] = useState(0);
  const [hasMore, setHasMore] = useState(false);

  const [searchInput, setSearchInput] = useState("");
  const [loadedSearch, setLoadedSearch] = useState([]);
  const [pageNumberSearch, setPageNumberSearch] = useState(1);
  const [hasMoreSearch, setHasMoreSearch] = useState(false);
  const [doSearch, setDoSearch] = useState(false);

  useEffect(() => { 
    setSearchInput("");
    var page = props.match.params.pageNumber;
    page = Number(page);
    if (isNaN(page) || page === 0)
    {
      page = 1;
    }
    console.log(page);
    setPageNumber(page);
  }, []);// eslint-disable-line react-hooks/exhaustive-deps

  useEffect(() => { 
    const fetchMoreData = async () => {
      var loadedMovieList = await getAllMovies(pageNumber);
      if (loadedMovieList.length === 0 || loadedMovieList.length < 48) {
        setHasMore(false);
      }
      else
      {
        setHasMore(true);
      }
      setLoadedMoives([...loadedMoives, ...loadedMovieList]);
    }
    if (pageNumber > 0) {
      fetchMoreData();
    }
  }, [pageNumber]);

  useEffect(() => { 
    if (doSearch) {// eslint-disable-next-line react-hooks/exhaustive-deps
      searchMovie(true);// eslint-disable-next-line react-hooks/exhaustive-deps
    }
  }, [pageNumberSearch]);

  useEffect(() => { 
    if (doSearch && searchInput.length === 0)// eslint-disable-next-line react-hooks/exhaustive-deps
    {
      setDoSearch(false);
      setHasMoreSearch(false);
      setHasMore(false);
      setLoadedMoives([]);
      setLoadedSearch([]);
      setPageNumber(1);
      setPageNumberSearch(1);
    }
  }, [searchInput]);

  const searchMovie = async (more) => { 
    if (searchInput.length > 0) {
      setDoSearch(true);
      if (more) {
        let searchResult = await search(searchInput, pageNumberSearch);
        if (searchResult.length === 0 || searchResult.length < 48) {
          setHasMoreSearch(false);
        }
        else {
          setHasMoreSearch(true);
        }
        setLoadedSearch([...loadedSearch, ...searchResult]);
      }
      else {
        let searchResult = await search(searchInput, 1);
        if (searchResult.length === 0 || searchResult.length < 48) {
          setHasMoreSearch(false);
        }
        else {
          setHasMoreSearch(true);
        }
        setLoadedSearch(searchResult);
      }
    }
  }

  function RenderMovieCards() {
    return doSearch ?
    loadedSearch.map((item) => {
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
    :
    loadedMoives.map((item) => {
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
      <Button justIcon round color="white" onClick = {() => searchMovie(false)}>
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
                <h1 className={classes.title}> </h1>
                <h3 className={classes.subtitle}> </h3>
              </div>
            </GridItem>
          </GridContainer>
        </div>
      </Parallax>
      <div className={classNames(classes.main, classes.mainRaised)}>
        <InfiniteScroll
          dataLength={doSearch ? loadedSearch.length : loadedMoives.length}
          next={() => { doSearch ? setPageNumberSearch(pageNumberSearch + 1) : setPageNumber(pageNumber + 1) }}
          hasMore={doSearch ? hasMoreSearch : hasMore}
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
